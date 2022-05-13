using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class ElementOperation<TItem>
    {
        //Чё то туплю с enum в итоге 1 это добавляем а 2 удаляем
        public TItem Item { get; set; }
        public int ReducedOperation { get; set; }
        public int Index { get; set; }
        public ElementOperation(TItem item, int op, int index)
        {
            Item = item;
            ReducedOperation = op;
            Index = index;
        }
    }

    public class ListModel<TItem>
    {
        public List<TItem> Items { get; set; }
        public int Limit;
        public LimitedSizeStack<ElementOperation<TItem>> Operation;
        //создаёт лист
        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Operation = new LimitedSizeStack<ElementOperation<TItem>>(limit);
        }
        //добавить итем
        public void AddItem(TItem item)
        {
            ElementOperation<TItem> elementAdd = new ElementOperation<TItem>(item, 1, Items.Count);
            Operation.Push(elementAdd);
            Items.Add(item);
        }
        //удалить итем
        public void RemoveItem(int index)
        {
            ElementOperation<TItem> elementRemove = new ElementOperation<TItem>(Items[index], 2, index);
            Operation.Push(elementRemove);
            Items.RemoveAt(index);
        }

        public bool CanUndo()
        {
            if (Operation.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Undo()
        {
            if (CanUndo())
            {
                var changeableAction = Operation.Pop();
                if (changeableAction.ReducedOperation == 1)
                {
                    Items.RemoveAt(Items.Count - 1);
                }
                else if (changeableAction.ReducedOperation == 2)
                {
                    Items.Insert(changeableAction.Index, changeableAction.Item);
                }
            }
        }
    }
}}
