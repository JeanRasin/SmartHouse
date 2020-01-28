using Bogus;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BogusTest
{
    //  https://github.com/bchavez/Bogus#f-and-vbnet-examples
    class Program
    {
        public enum Gender
        {
            Male,
            Female
        }
        internal class Order
        {
            public int OrderId { get; set; }
            //public object Item { get; internal set; }
            //public object Quantity { get; internal set; }
            //public object LotNumber { get; internal set; }
        }

        static void Main(string[] args)
        {
            string[] fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

            int orderIds = 0;
            var testOrders = new Faker<Order>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode
                // .StrictMode(true)
                //OrderId is deterministic
                .RuleFor(o => o.OrderId, f => f.Random.Number(1, 5))
                .FinishWith((f, u) =>
                {
                    Console.WriteLine("User Created! Id={0}", u.OrderId);
                });
            //Pick some fruit from a basket
            //.RuleFor(o => o.Item, f => f.PickRandom(fruit))
            ////A random quantity from 1 to 10
            //.RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
            ////A nullable int? with 80% probability of being null.
            ////The .OrNull extension is in the Bogus.Extensions namespace.
            //.RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .8f)
            // );


            List<Order> data = testOrders.Generate(20);
            Console.WriteLine(JsonSerializer.Serialize(data));
        }
    }
}
