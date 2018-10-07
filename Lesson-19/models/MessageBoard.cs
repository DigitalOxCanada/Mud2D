using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Mud2D.models
{
    public class MessageBoard
    {
        private class Message
        {
            public DateTime Added { get; set; }
            public string Note { get; set; }
        }

        private bool Dirty { get; set; }
        private Queue<Message> Messages { get; set; }
        private DateTime LastAdded { get; set; }
        private const int MaxLines = 5;
        private const int RollingSpeed = 6;

        public MessageBoard()
        {
            Messages = new Queue<Message>();
        }

        public void Add(string msg)
        {
            DateTime now = DateTime.Now;
            Messages.Enqueue(new Message() { Added = now, Note = $"[{now.ToString("H:mm:ss")}] {msg}" });
            Dirty = true;
            LastAdded = now;
        }

        public void Update()
        {
            Console.SetCursorPosition(0, 20);
            DateTime now = DateTime.Now;
            //this provides a degredation of the message board even if no new messages are coming in.
            if((now-LastAdded).Seconds > RollingSpeed)
            {
                Dirty = true;
                LastAdded = now;
            }
            if (Dirty)
            {
                Console.WriteLine("******* Messages *******");
                int cnt = 0;
                foreach (var msg in Messages.OrderByDescending(p => p.Added))
                {
                    if (++cnt > MaxLines) break;
                    //Console.ForegroundColor = colors[cnt-1];
                    int age = (now - msg.Added).Seconds;
                    if (age > 20) Console.ForegroundColor = ConsoleColor.DarkGray;
                    else if (age > 15) Console.ForegroundColor = ConsoleColor.Gray;
                    else if (age > 5) Console.ForegroundColor = ConsoleColor.White;
                    else Console.ForegroundColor = ConsoleColor.DarkGreen;

                    Console.WriteLine(msg.Note.PadRight(60));
                }
                Dirty = false;
                Console.ResetColor();
            }

            //after so many items we should prune the stack so we don't hit a memory limit.
            if(Messages.Count>20)
            {
                while(Messages.Count>20)
                {
                    Messages.Dequeue();
                }
            }
        }
    }
}
