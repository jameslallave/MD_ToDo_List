namespace MD_ToDo_List;
using System.Collections.ObjectModel;

public partial class MainPage : ContentPage
{
    private ObservableCollection<ToDoClass> toDoList = new ObservableCollection<ToDoClass>();
    private ToDoClass? selectedToDo = null;
    private int nextId = 1;

    public MainPage()
    {
        InitializeComponent();
        todoLV.ItemsSource = toDoList;
        toDoList.CollectionChanged += (s, e) => UpdateUI();
    }

    private void AddToDoItem(object sender, EventArgs e)
    {
        string title = titleEntry.Text?.Trim() ?? string.Empty;
        string detail = detailsEditor.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(title))
        {
            DisplayAlert("Validation", "Please enter a title for the task.", "OK");
            return;
        }

        toDoList.Add(new ToDoClass { id = nextId++, title = title, detail = detail });
        ClearInputs();
    }

    private void todoLV_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is ToDoClass tapped)
        {
            selectedToDo = tapped;
            titleEntry.Text = tapped.title;
            detailsEditor.Text = tapped.detail;

            addBtn.IsVisible = false;
            editBtn.IsVisible = true;
            cancelBtn.IsVisible = true;
        }
    }

    private void TodoLV_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        todoLV.SelectedItem = null;
    }

    private void EditToDoItem(object sender, EventArgs e)
    {
        if (selectedToDo == null) return;

        string title = titleEntry.Text?.Trim() ?? string.Empty;
        string detail = detailsEditor.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(title))
        {
            DisplayAlert("Validation", "Title cannot be empty.", "OK");
            return;
        }

        selectedToDo.title = title;
        selectedToDo.detail = detail;

        todoLV.ItemsSource = null;
        todoLV.ItemsSource = toDoList;

        CancelEdit(sender, e);
    }

    private void CancelEdit(object sender, EventArgs e)
    {
        selectedToDo = null;
        ClearInputs();
        addBtn.IsVisible = true;
        editBtn.IsVisible = false;
        cancelBtn.IsVisible = false;
    }

    private void DeleteToDoItem(object sender, EventArgs e)
    {
        if (sender is Button btn && int.TryParse(btn.ClassId, out int itemId))
        {
            var item = toDoList.FirstOrDefault(t => t.id == itemId);
            if (item != null)
            {
                toDoList.Remove(item);
                if (selectedToDo?.id == itemId)
                    CancelEdit(sender, e);
            }
        }
    }

    private void ClearInputs()
    {
        titleEntry.Text = string.Empty;
        detailsEditor.Text = string.Empty;
    }

    private void UpdateUI()
    {
        bool hasTasks = toDoList.Count > 0;
        emptyState.IsVisible = !hasTasks;
        taskCountLabel.Text = hasTasks
            ? $"{toDoList.Count} task{(toDoList.Count == 1 ? "" : "s")}"
            : "No tasks yet";
    }
}