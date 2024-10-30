using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using UserManagement;

namespace UserManagement
{
    public class AuthService
    {
        private readonly string jsonFilePath = "user_data.json";
        private readonly Encryptor encryptor;

        public AuthService()
        {
            encryptor = new Encryptor();
        }

        public bool Login(string login, string password, string phoneNumber, int userId)
        {
            // ������ ������ �� JSON �����
            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException("User  data file not found.");
            }

            var json = File.ReadAllText(jsonFilePath);
            var users = JsonSerializer.Deserialize<User[]>(json);

            // ����� ������������
            var user = users?.FirstOrDefault(u =>
                u.login == login &&
                u.userId == userId &&
                u.phoneNumber == phoneNumber);

            if (user == null)
            {
                return false; // ������������ �� ������
            }

            // ���������� ���������� ������ ��� ���������
            var encryptedPassword = encryptor.Encrypt(password);

            // �������� ������
            if (encryptedPassword == user.GetPassword())
            {
                // ������������ ������ � ������ ���������
                return true; // �������� ����
            }

            return false; // �������� ������
        }
    }
}
