using System;
using System.Collections.Generic;
using System.Linq;

namespace Module12
{
    public abstract class Car
    {
        protected Random random = new Random();
        public string Name { get; protected set; }
        public int Speed { get; protected set; }
        public int DistanceCovered { get; protected set; }

        public delegate void CarEventHandler(Car car);
        public event CarEventHandler Finished;

        protected Car(string name)
        {
            Name = name;
            DistanceCovered = 0;
        }

        public void Race()
        {
            Speed = GetRandomSpeed();
            DistanceCovered += Speed;

            if (DistanceCovered >= 100)
            {
                OnFinished();
            }
        }

        protected virtual void OnFinished()
        {
            Finished?.Invoke(this);
        }

        protected virtual int GetRandomSpeed()
        {
            return random.Next(1, 100); // Default random speed
        }
    }

    public class SportsCar : Car
    {
        public SportsCar(string name) : base(name) { }

        protected override int GetRandomSpeed()
        {
            return random.Next(50, 150); // Faster speed range for sports cars
        }
    }

    public class Truck : Car
    {
        public Truck(string name) : base(name) { }

        protected override int GetRandomSpeed()
        {
            return random.Next(10, 80); // Slower speed range for trucks
        }
    }

    public class Bus : Car
    {
        public Bus(string name) : base(name) { }

        protected override int GetRandomSpeed()
        {
            return random.Next(10, 70); // Slower speed range for buses
        }
    }

    public class RaceGame
    {
        private List<Car> cars = new List<Car>();

        public void AddCar(Car car)
        {
            car.Finished += Car_Finished;
            cars.Add(car);
        }

        private void Car_Finished(Car car)
        {
            Console.WriteLine($"{car.Name} has finished the race!");
            // Stop the race for this car
            car.Finished -= Car_Finished;
        }

        public void StartRace()
        {
            Console.WriteLine("Race started!");
            while (cars.Any(c => c.DistanceCovered < 100))
            {
                foreach (var car in cars)
                {
                    car.Race();
                    Console.WriteLine($"{car.Name} is at {car.DistanceCovered}");
                    System.Threading.Thread.Sleep(500); // Delay for visualization
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var race = new RaceGame();
            race.AddCar(new SportsCar("BMW"));
            race.AddCar(new Truck("AMG"));
            race.AddCar(new Bus("Honda"));

            race.StartRace();
            Console.ReadKey();
        }
    }
}
