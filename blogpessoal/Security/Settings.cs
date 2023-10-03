namespace blogpessoal.Security
{
    public class Settings
    {
        private static string secret = "431d40325f11de8a3dac4d47903da061a55fead5abdc8743ea083272846b8658";

        public static string Secret { get => secret; set => secret = value; }
    }
}
