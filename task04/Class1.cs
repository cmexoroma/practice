namespace task04;

public interface ISpaceship
{
    void MoveForward();      // Движение вперед
    void Rotate(int angle);  // Поворот на угол (градусы)
    void Fire();             // Выстрел ракетой
    int Speed { get; }       // Скорость корабля
    int FirePower { get; }   // Мощность выстрела
}

public class Cruiser : ISpaceship
{
    public int Speed { get; }
    public int FirePower { get; }

    public int Distance { get; private set; }
    public int Angle { get; private set; }
    public int Bullets { get; private set; }

    public Cruiser()
    {
        Speed = 50;
        FirePower = 100;
        Distance = 0;
        Angle = 0;
        Bullets = 45;
    }

    public void MoveForward()
    {
        Distance += Speed;
    }

    public void Rotate(int angle)
    {
        Angle = (Angle + angle) % 360;
    }

    public void Fire()
    {
        if(Bullets > 0)
        {
            Bullets -= 1;
        }
    }
}

public class Fighter : ISpaceship
{
    public int Speed { get; }
    public int FirePower { get; }

    public int Distance { get; private set; }
    public int Angle { get; private set; }
    public int Bullets { get; private set; }

    public Fighter()
    {
        Speed = 100;
        FirePower = 50;
        Distance = 0;
        Angle = 0;
        Bullets = 25;
    }

    public void MoveForward()
    {
        Distance += Speed;
    }

    public void Rotate(int angle)
    {
        Angle = (Angle + angle) % 360;
    }

    public void Fire()
    {
        if(Bullets > 0)
        {
            Bullets -= 1;
        }
    }
}
