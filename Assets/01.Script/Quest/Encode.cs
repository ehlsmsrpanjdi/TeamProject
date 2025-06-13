using System;
using System.Security.Cryptography;
using System.Text;

public static class Encode
{
    private static readonly string key = "UnsecureExample1";
    private static readonly string iv = "HardcodingKeyBad";

    public static string Encrypt(string plainText)
    {
        using Aes aes = Aes.Create(); // C# 8 이상에서 사용. Dispose() 자동 호출. 암호화에 필요한 객체 생성.
        aes.Key = Encoding.UTF8.GetBytes(key); // 암호화/복호화 시 사용, 틀리면 암/복호화 안됨.
        aes.IV = Encoding.UTF8.GetBytes(iv); // 암호화 할 때와 복호화 할 때 IV값이 틀리면 저장 값 비정상 복호화.

        using var encryptor = aes.CreateEncryptor(); // 실제 암호화 담당 객체 생성
        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText); // 저장할 값을 Byte 단위로 분해
        byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        // 암호화 진행하여 바이너리 데이터(byte[]) 형식. TransformFinalBlock(암호화할 데이터, 암호화 시작위치, 암호화 할 끝 위치)

        // encryptedBytes는 바이너리 데이터(텍스트 아님). json은 텍스트 데이터(UTF-8), 바이너리 데이터를 json에 바로 넣으면 깨질 수 있으니
        // 텍스트 형식(Base64)로 변환해서 리턴하면 암호화 시 변환까지 됐으니 추가 작업 없이 이용 가능함.
        return Convert.ToBase64String(encryptedBytes);
    }

    public static string Decrypt(string encryptedText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = Encoding.UTF8.GetBytes(iv);

        using var decryptor = aes.CreateDecryptor();
        byte[] inputBytes = Convert.FromBase64String(encryptedText);
        byte[] decryptedBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
