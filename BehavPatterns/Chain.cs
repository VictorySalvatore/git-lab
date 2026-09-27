using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavPatterns
{
    public enum RequestType // Набор именованных констант
    {
        TypeA,
        TypeB,
        TypeC
    }
    public class Request // Запрос для цепочки
    {
        public RequestType Type { get; }
        public Request(RequestType type) { Type = type; }
    }
    public interface Handler // Интерфейс обработчика
    {
        void HandleRequest(Request request);
        void SetNextHandler(Handler nextHandler);
    }
    public class ConcreteHandlerA : Handler // Обрабатывает запросы типа TypeA
    {
        private Handler nextHandler;
        public void HandleRequest(Request request)
        {
            if (request.Type == RequestType.TypeA)
                Console.WriteLine("ConcreteHandlerA handled the request.");
            else if (nextHandler != null)
                nextHandler.HandleRequest(request);
            else
                Console.WriteLine($"Request {request.Type} was not handled.");
        }
        public void SetNextHandler(Handler nextH) => nextHandler = nextH;
    }
    public class ConcreteHandlerB : Handler // Обрабатывает запросы типа TypeB
    {
        private Handler nextHandler;
        public void HandleRequest(Request request)
        {
            if (request.Type == RequestType.TypeB)
                Console.WriteLine("ConcreteHandlerB handled the request.");
            else if (nextHandler != null)
                nextHandler.HandleRequest(request);
            else
                Console.WriteLine($"Request {request.Type} was not handled.");
        }
        public void SetNextHandler(Handler nextH) => nextHandler = nextH;
    }
    public class ConcreteHandlerC : Handler // Обрабатывает запросы типа TypeC
    {
        private Handler nextHandler;
        public void HandleRequest(Request request)
        {
            if (request.Type == RequestType.TypeC)
                Console.WriteLine("ConcreteHandlerC handled the request.");
            else if (nextHandler != null)
                nextHandler.HandleRequest(request);
            else
                Console.WriteLine($"Request {request.Type} was not handled.");
        }
        public void SetNextHandler(Handler nextH) => nextHandler = nextH;
    }
}
