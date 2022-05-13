using System;
using System.Collections.Generic;

namespace Clones
{
    public class Stack
    {
        private StackItem last;
        public Stack() { }

        public Stack(Stack stack)
        {
            last = stack.last;
        }

        public void Push(int value)
        {
            last = new StackItem(value, last);
        }

        public int Peek()
        {
            return last.Value;
        }

        public int Pop()
        {
            var value = last.Value;
            last = last.Previous;
            return value;
        }

        public bool IsEmpty()
        {
            return last == null;
        }

        public void Clear()
        {
            last = null;
        }
    }

    public class StackItem
    {
        public readonly int Value;
        public readonly StackItem Previous;

        public StackItem(int value, StackItem previous)
        {
            Value = value;
            Previous = previous;
        }
    }

    public class Clone
    {
        public Stack LearnProgramm;
        public Stack RelearnProgramm;
        public Clone()
        {
            LearnProgramm = new Stack();
            RelearnProgramm = new Stack();
        }

        public void Learn(int command)
        {
            RelearnProgramm.Clear();
            LearnProgramm.Push(command);
        }

        public void RollBack()
        {
            RelearnProgramm.Push(LearnProgramm.Pop());
        }

        public void Relearn()
        {
            LearnProgramm.Push(RelearnProgramm.Pop());
        }

        public Clone(Clone nextClone)
        {
            LearnProgramm = new Stack(nextClone.LearnProgramm);
            RelearnProgramm = new Stack(nextClone.RelearnProgramm);
        }

        public string Check()
        {
            return LearnProgramm.IsEmpty() ? "basic" : LearnProgramm.Peek().ToString();
        }
    }

    public class CloneVersionSystem : ICloneVersionSystem
    {
        public static Dictionary<int, Clone> DictionaryClone;
        public CloneVersionSystem()
        {
            DictionaryClone = new Dictionary<int, Clone> { { 0, new Clone() } };
        }

        public string Execute(string query)
        {
            var commandArray = query.Split();
            string nameCommand = commandArray[0];
            int namberClone = Convert.ToInt32(commandArray[1]) - 1;
            int comand = nameCommand == "learn" ? Convert.ToInt32(commandArray[2]) : 0;
            switch (nameCommand)
            {
                case "learn":
                    DictionaryClone[namberClone].Learn(comand);
                    return null;
                case "rollback":
                    DictionaryClone[namberClone].RollBack();
                    return null;
                case "relearn":
                    DictionaryClone[namberClone].Relearn();
                    return null;
                case "clone":
                    DictionaryClone.Add(DictionaryClone.Count, new Clone(DictionaryClone[namberClone]));
                    return null;
                case "check":
                    return DictionaryClone[namberClone].Check();
            }
            return null;
        }
    }
}