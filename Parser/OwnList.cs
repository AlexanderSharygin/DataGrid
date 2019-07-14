using System;


namespace Parser.Extensions
{
   class OwnList<TypeOfList>
    {
        private TypeOfList[] List;
        public OwnList()
        {
            List = new TypeOfList[0];
        }
        public TypeOfList this[int elementIndex]
        {
            get
            {
                ThrowIfInvalidInsertIndex(elementIndex);
                return List[elementIndex];
            }
            set
            {
                ThrowIfInvalidInsertIndex(elementIndex);
                List[elementIndex] = value;
            }
        }
        private void ChangeListSize(int newSize)
        {
            TypeOfList[] temp = new TypeOfList[newSize];
            for (int i = 0; i < List.Length; i++)
            {
                temp[i] = List[i];
            }
            List = temp;
        }
        private void InsertByIndex(int index, TypeOfList item)
        {
            ThrowIfInvalidInsertIndex(index);
            ChangeListSize(List.Length + 1);
            for (var i = List.Length - 1; i > index; i--)
            {
                List[i] = List[i - 1];
            }
            List[index] = item;
        }
        private void ThrowIfInvalidInsertIndex(int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex > List.Length))
            {
                throw new IndexOutOfRangeException("Попытка добавить/получить элемент в список типа MyList с недопустимым индексом");
            }
        }
        public int IndexOf(TypeOfList x)
        {
            int itemFirstIndex = 0;
            foreach (var item in List)
            {
                if (item.Equals(x))
                {
                    return itemFirstIndex;
                }
                itemFirstIndex++;
            }
            return -1;
        }
        public void AddFirst(TypeOfList item) => InsertByIndex(0, item);
        public void AddByIndex(int index, TypeOfList item) => InsertByIndex(index, item);
        public void AddLast(TypeOfList item) => InsertByIndex(List.Length, item);
        public void Resize(int newSize) => ChangeListSize(newSize);
        public int Length => List.Length;
    }
}
