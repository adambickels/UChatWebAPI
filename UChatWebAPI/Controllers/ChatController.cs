using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using UChatWebAPI.Models;

namespace UChatWebAPI.Controllers
{
    public class ChatController : ApiController
    {
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public Message ReceiveMessage()
        {
            try
            {
                RabbitMQLogic obj = new RabbitMQLogic();
                IConnection con = obj.GetConnection();

                string msgStr = obj.Receive(con);
                Message message = JsonConvert.DeserializeObject<Message>(msgStr);

                return message;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public bool SendMessage(Message message)
        {
            RabbitMQLogic obj = new RabbitMQLogic();
            IConnection con = obj.GetConnection();

            string JSON = JsonConvert.SerializeObject(message);

            return obj.Send(con, JSON);
        }
    }
}
