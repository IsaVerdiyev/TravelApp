using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Specifications;

namespace TravelAppCore.Services
{
    public class CheckListService : ICheckListService
    {

        IRepository<ToDoItem> checkListRepository;

        public CheckListService(IRepository<ToDoItem> checkListRepository)
        {
            this.checkListRepository = checkListRepository;
        }

        public ToDoItem AddItemInCheckList(Trip trip, ToDoItem toDoItem)
        {
            toDoItem.TripId = trip.Id;
            return checkListRepository.Add(toDoItem);
        }

        public async Task<ToDoItem> AddItemInCheckListAsync(Trip trip, ToDoItem toDoItem)
        {
            toDoItem.TripId = trip.Id;
            return await checkListRepository.AddAsync(toDoItem);
        }

        public ToDoItem ChangeCheckedStateOfToDoItem(ToDoItem toDoItem, bool state)
        {
            toDoItem.Done = state;
            checkListRepository.Update(toDoItem);
            return toDoItem;
        }

        public async Task<ToDoItem> ChangeCheckedStateOfToDoItemAsync(ToDoItem toDoItem, bool state)
        {
            toDoItem.Done = state;
            await checkListRepository.UpdateAsync(toDoItem);
            return toDoItem;
        }

        public ToDoItem ChangeNameOfToDoItem(ToDoItem toDoItem, string name)
        {
            toDoItem.Name = name;
            checkListRepository.Update(toDoItem);
            return toDoItem;
        }

        public async Task<ToDoItem> ChangeNameOfToDoItemAsync(ToDoItem toDoItem, string name)
        {
            toDoItem.Name = name;
            await checkListRepository.UpdateAsync(toDoItem);
            return toDoItem;
        }

        public void RemoveItemFromCheckList(DeleteByIdSpecification<ToDoItem> deleteByIdSpecification)
        {
            checkListRepository.DeleteBySpec(deleteByIdSpecification);
        }

        public async Task RemoveItemFromCheckListAsync(DeleteByIdSpecification<ToDoItem> deleteByIdSpecification)
        {
            await checkListRepository.DeleteBySpecAsync(deleteByIdSpecification);
        }
    }
}
