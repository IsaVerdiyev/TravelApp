using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    interface ICheckListService
    {
        ToDoItem AddItemInCheckList(Trip trip, ToDoItem toDoItem);
        Task<ToDoItem> AddItemInCheckListAsync(Trip trip, ToDoItem toDoItem);

        void RemoveItemFromCheckList(Trip trip, ToDoItem toDoItem);
        Task RemoveItemFromCheckListAsync(Trip trip, ToDoItem toDoItem);

        ToDoItem ChangeNameOfToDoItem(ToDoItem toDoItem, string name);
        Task<ToDoItem> ChangeNameOfToDoItemAsync(ToDoItem toDoItem, string name);

        ToDoItem ChangeCheckedStateOfToDoItem(ToDoItem toDoItem, bool state);
        Task<ToDoItem> ChangeCheckedStateOfToDoItemAsync(ToDoItem toDoItem, bool state);
    }
}
