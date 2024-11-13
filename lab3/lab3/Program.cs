using System;

public class Password
{
    private string _password;
 
    private const string DefaultPassword = "password123";

   
    public class Production
    {
        public int Id;
        public string OrganizationName;

        public Production(int id, string organizationName)
        {
            Id = id;
            OrganizationName = organizationName;
        }

        public override string ToString()
        {
            return $"Production: {OrganizationName} (Id: {Id})";
        }
    }

   
    public class Developer
    {
        public string FullName;
        public int DeveloperId;
        public string Department;

        public Developer(string fullName, int developerId, string department)
        {
            FullName = fullName;
            DeveloperId = developerId;
            Department = department;
        }

        public override string ToString()
        {
            return $"Developer: {FullName}, Id: {DeveloperId}, Department: {Department}";
        }
    }

    public Production ProductionInfo;
    public Developer DeveloperInfo;

    public Password(string password, int productionId, string organizationName, string developerName, int developerId, string department)
    {
        _password = password;

        // Инициализация вложенных объектов
        ProductionInfo = new Production(productionId, organizationName);
        DeveloperInfo = new Developer(developerName, developerId, department);
    }

    
    public int Length => _password.Length;

    
    public static bool operator >(Password p1, Password p2)
    {
        return p1._password.Length > p2._password.Length;
    }

    public static bool operator <(Password p1, Password p2)
    {
        return p1._password.Length < p2._password.Length;
    }

    
    public static bool operator ==(Password p1, Password p2)
    {
        return p1._password == p2._password;
    }

    public static bool operator !=(Password p1, Password p2)
    {
        return p1._password != p2._password;
    }

  
    public static Password operator ++(Password p)
    {
        p._password = DefaultPassword;
        return p;
    }

  
    public static Password operator -(Password p, char replacement)
    {
        if (p._password.Length > 0)
        {
            p._password = p._password.Substring(0, p._password.Length - 1) + replacement;
        }
        return p;
    }


    
    public static bool operator true(Password p)
    {
        return p._password.Length >= 8 && HasUpperCase(p._password) && HasLowerCase(p._password) && HasDigit(p._password);
    }

    public static bool operator false(Password p)
    {
        return !(p);
    }

    public static bool operator !(Password p)
    {
        return !(p._password.Length >= 8 && HasUpperCase(p._password) && HasLowerCase(p._password) && HasDigit(p._password));
    }

    // методы для проверки пароля на стойкость
    private static bool HasUpperCase(string password)
    {
        foreach (char c in password)
        {
            if (char.IsUpper(c))
                return true;
        }
        return false;
    }

    private static bool HasLowerCase(string password)
    {
        foreach (char c in password)
        {
            if (char.IsLower(c))
                return true;
        }
        return false;
    }

    private static bool HasDigit(string password)
    {
        foreach (char c in password)
        {
            if (char.IsDigit(c))
                return true;
        }
        return false;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Password p = (Password)obj;
        return _password == p._password;
    }

    public override int GetHashCode()
    {
        return _password.GetHashCode();
    }

    public override string ToString()
    {
        return _password;
    }
}

// Методы расширения
public static class PasswordExtensions
{
   
    public static char GetMiddleCharacter(this Password p)
    {
        if (p.ToString().Length == 0)
            return '\0';  
        int middleIndex = p.ToString().Length / 2;
        return p.ToString()[middleIndex];
    }

    
    public static bool IsLengthValid(this Password p)
    {
        int length = p.ToString().Length;
        return length >= 6 && length <= 12;
    }
}

public static class StatisticOperation
{
    
    public static int Sum(Password p1, Password p2)
    {
        return p1.Length + p2.Length;
    }

   
    public static int Difference(Password p1, Password p2)
    {
        return Math.Abs(p1.Length - p2.Length);
    }

    
    public static int CountCharacters(Password p)
    {
        return p.Length;
    }



public static bool IsStrong(this Password p)
{
    string passString = p.ToString();
    return passString.HasUpperCase() && passString.HasDigit();
}
    
    public static bool HasUpperCase(this string str)
    {
        foreach (char c in str)
        {
            if (char.IsUpper(c))
            {
                return true;
            }
        }
        return false;
    }

    
    public static bool HasDigit(this string str)
    {
        foreach (char c in str)
        {
            if (char.IsDigit(c))
            {
                return true;
            }
        }
        return false;
    }

    public static bool HasLowerCase(this string str)
    {
        foreach (char c in str)
        {
            if (char.IsLower(c))
            {
                return true;
            }
        }
        return false;
    }

    
    public static int CountDigits(this string str)
{
    int count = 0;
    foreach (char c in str)
    {
        if (char.IsDigit(c))
        {
            count++;
        }
    }
    return count;
}


}


public class Program
{
    public static void Main()
    {
        
        Password password1 = new Password("Pass123", 1, "Halex", "Helen Roe", 101, "Development");
        Password password2 = new Password("pa3ssr", 2, "InnovateInc", "Jane Smith", 102, "Research");
        Console.WriteLine("Password1 Production Info: " + password1.ProductionInfo);
        Console.WriteLine("Password1 Developer Info: " + password1.DeveloperInfo);

        Console.WriteLine("Password2 Production Info: " + password2.ProductionInfo);
        Console.WriteLine("Password1 Developer Info: " + password2.DeveloperInfo);

        Console.WriteLine("-------------------------------------------------------------");

        
        Console.WriteLine($"Пароль 1 > Пароль 2: {password1 > password2}");
        Console.WriteLine($"Пароль 1 < Пароль 2: {password1 < password2}");
        Console.WriteLine("-------------------------------------------------------------");

       
        Console.WriteLine($"Пароль 1 == Пароль 2: {password1 == password2}");
        Console.WriteLine($"Пароль 1 != Пароль 2: {password1 != password2}");
        Console.WriteLine("-------------------------------------------------------------");

        
        Console.WriteLine("Сброс пароля...");
        password1++;
        Console.WriteLine($"Новый пароль (по умолчанию): {password1}");
        Console.WriteLine("-------------------------------------------------------------");

        

        Console.WriteLine($"Пароль2 слабый? {!password2}");
        Console.WriteLine("-------------------------------------------------------------");

        
        Console.WriteLine("Замена последнего элемента в Пароле 1 на 'Z'...");
        password1 = password1 - 'Z';

        Console.WriteLine("пароль после замены: " + password1);
        Console.WriteLine("-------------------------------------------------------------");

        // Тестирование методов расширения
        Console.WriteLine($"Выделение среднего символа из пароля1: {password1.GetMiddleCharacter()}");
        Console.WriteLine($"Выделение среднего символа из пароля2: {password2.GetMiddleCharacter()}");
        Console.WriteLine("-------------------------------------------------------------");

        Console.WriteLine($"Допустима ли длина пароля2? {password2.IsLengthValid()}");
        Console.WriteLine($"Допустима ли длина пароля1? {password1.IsLengthValid()}");
        Console.WriteLine("-------------------------------------------------------------");

        Console.WriteLine($"Пароль2 содержит {password2.ToString().CountDigits()} цифр(ы).");
        Console.WriteLine($"Пароль1 содержит {password1.ToString().CountDigits()} цифр(ы).");
        Console.WriteLine("-------------------------------------------------------------");

        // Использование методов статистических операций
        Console.WriteLine($"Сумма длин пароля1 и пароля2: {StatisticOperation.Sum(password1, password2)}");
        Console.WriteLine($"Разность длин пароля1 и пароля2: {StatisticOperation.Difference(password1, password2)}");
        Console.WriteLine("-------------------------------------------------------------");

        Console.WriteLine($"Количество символов в пароле 1: {StatisticOperation.CountCharacters(password1)}");
        Console.WriteLine("-------------------------------------------------------------");

        // Проверка на стойкость с использованием метода IsStrong
        Console.WriteLine($"Надежный ли пароль1 (метод расширения)? {password1.IsStrong()}");
    }
}