using System;
using System.Collections.Generic;

namespace POE_Part2  // FIXED
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;

        public TaskManager()
        {
            _storage = new TaskStorageHelper();
        }

        public string AddTask(string title, string description, string reminder)
        {
            Task newTask = new Task
            {
                Title = title,
                Description = description,
                Reminder = reminder,
                IsComplete = false,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };

            _storage.AddTask(newTask);
            ActivityLogger.Log($"Task added: '{title}'" + (string.IsNullOrEmpty(reminder) ? "" : $" (Reminder set for {reminder})"));
            return $"Task '{title}' added successfully!";
        }

        public List<Task> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        public string MarkAsComplete(int id)
        {
            _storage.MarkAsComplete(id);
            ActivityLogger.Log($"Task marked complete: ID {id}");
            return "Task marked as complete!";
        }

        public string DeleteTask(int id)
        {
            _storage.DeleteTask(id);
            ActivityLogger.Log($"Task deleted: ID {id}");
            return "Task deleted successfully!";
        }
    }
}