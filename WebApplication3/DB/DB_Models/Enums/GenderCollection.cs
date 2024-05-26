namespace WebApplication3.DB.DB_Models;

public class GenderCollection
{
    public static Dictionary<string, int> Gender
    {
        get
        {
            var FontSize = new Dictionary<string, int>();
            foreach (var text in Enum.GetNames(typeof(Gender)))
            {
                FontSize.Add(text, (int)Enum.Parse(typeof(Gender), text));
            }
            return FontSize;
        }
    }
}
public enum Gender
{
    Female = 1,
    Male = 2
}