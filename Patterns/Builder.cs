using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Builder
{
    public class Pizza // Продукт
    {
        public string Dough { get; set; }
        public string Sauce { get; set; }
        public string Topping { get; set; }
        public override string ToString() =>
            $"Pizza{{dough='{Dough}', sauce='{Sauce}', topping='{Topping}'}}";
    }
    public interface PizzaBuilder // Интерфейс строителя
    {
        void BuildDough();
        void BuildSauce();
        void BuildTopping();
        Pizza GetResult();
    }
    public class HawaiianPizzaBuilder : PizzaBuilder // Конкретные строители
    {
        private readonly Pizza pizza = new Pizza();
        public void BuildDough() => pizza.Dough = "cross";
        public void BuildSauce() => pizza.Sauce = "mild";
        public void BuildTopping() => pizza.Topping = "ham+pineapple";
        public Pizza GetResult() => pizza;
    }
    public class PepperoniPizzaBuilder : PizzaBuilder
    {
        private readonly Pizza pizza = new Pizza();
        public void BuildDough() => pizza.Dough = "thin";
        public void BuildSauce() => pizza.Sauce = "tomato";
        public void BuildTopping() => pizza.Topping = "pepperoni+cheese";
        public Pizza GetResult() => pizza;
    }
    public class PizzaDirector // Директор — задаёт порядок шагов
    {
        private readonly PizzaBuilder builder;
        public PizzaDirector(PizzaBuilder builder)
        {
            this.builder = builder;
        }
        public void ConstructPizza()
        {
            builder.BuildDough();
            builder.BuildSauce();
            builder.BuildTopping();
        }
    }
}
