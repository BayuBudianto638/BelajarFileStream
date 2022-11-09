using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BelajarFileStream
{
    class Program
    {
        static  void Main()
        {
            List<Dog> dogs = new List<Dog>();

            Dog myDog = new Dog();
            myDog.Name = "Chihuahua";
            myDog.Color = DogColor.Brown;
            dogs.Add(myDog);

            Dog myDog2 = new Dog();
            myDog2.Name = "Doge Coin";
            myDog2.Color = DogColor.Black;
            dogs.Add(myDog2);

            string filePath = @"C:\Error\MyDog.dat";
            MemoryStream memStream = SerializeToStream(dogs);
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            memStream.Seek(0, SeekOrigin.Begin); //membaca posisi object di memory dari posisi awal
            memStream.CopyTo(fs); // mengcopy isi memory stream ke filestream
            fs.Flush();
            fs.Close();

            MemoryStream memoryStream = new MemoryStream();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileStream.CopyTo(memoryStream);

            List<Dog> newDog =  (List<Dog>)DeserializeFromStream(memoryStream);
            foreach (var dog in newDog)
            {
                Console.WriteLine($"Dog : {dog.Name} - {dog.Color}");
            }
        }

        // Memorystream itu membuat stream ke dalam memory
        public static MemoryStream SerializeToStream(object obj)
        {
            MemoryStream ms = new MemoryStream();
            var formatter = new BinaryFormatter(); // Merubah object menjadi biner
            formatter.Serialize(ms, obj);
            return ms;
        }

        // Merubah stream ke object
        public static object DeserializeFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object obj = formatter.Deserialize(stream);
            return obj;
        }
    }

    enum DogColor
    {
        Brown,
        Black,
        White
    }

    // Serializable = objek di serialiasi ke stream yang membawa data(objek), stream jg memiliki informasi tentang type object. 
    // Dari stream tersebut sebuah objek bisa disimpan di db, file atau memory
    [Serializable] 
    class Dog
    {
        public string Name { get; set; }
        public DogColor Color { get; set; }

        public override string ToString()
        {
            return String.Format("Dog : {0} {1}", Name, Color);
        }
    }
}