using POE_Part2;
using System.Collections.Generic;
using System.Linq;

public class TaskStorageHelper
{
    private readonly ApplicationDbContext db;

    public TaskStorageHelper()
    {
        db = new ApplicationDbContext();

        // THIS CREATES THE DATABASE IF IT DOESN'T EXIST
        db.EnsureDatabaseCreated();
    }

    public void AddTask(Task task)
    {
        db.Tasks.Add(task);
        db.SaveChanges();
    }

    public List<Task> LoadTasks()
    {
        return db.Tasks.ToList();
    }

    public void MarkAsComplete(int id)
    {
        var task = db.Tasks.Where(t => t.Id == id).FirstOrDefault();
        if (task != null)
        {
            task.IsComplete = true;
            db.Tasks.Update(task);
            db.SaveChanges();
        }
    }

    public void DeleteTask(int id)
    {
        var task = db.Tasks.Where(t => t.Id == id).FirstOrDefault();
        if (task != null)
        {
            db.Tasks.Remove(task);
            db.SaveChanges();
        }
    }
}