const uri = '/api/Todo';
let todos = [];

function getItems() {
    let pageSize = document.getElementById("page-size");
    let page = document.getElementById("page");

    fetch(`${uri}/?size=${pageSize.value}&page=${page.value}`)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const addDescTextbox = document.getElementById('add-desc');
    const addDeadlineTextbox = document.getElementById('add-deadline');
    const addCompletion = document.getElementById('add-completion');
    console.log(addCompletion.checked)
    const item = {
        IsComplete: addCompletion.checked,
        Name: addNameTextbox.value.trim(),
        Description: addDescTextbox.value.trim() || null,
        Deadline: addDeadlineTextbox.value.trim() || null,
    };

    addNameTextbox.value = '';
    addDescTextbox.value = '';
    addDeadlineTextbox.value = '';
    addCompletion.checked = false;

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .then(() => _displayCount())
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .then(() => _displayCount())
        .catch(error => console.error('Unable to delete item.', error));
}

function changePage(change) {
    let page = document.getElementById("page");
    page.value = parseInt(page.value, 10) + parseInt(change, 10);
    getItems()
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);
    console.log(item)
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-desc').value = item.description;
    document.getElementById('edit-deadline').value = item.deadline;
    document.getElementById('edit-completion').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        Id: parseInt(itemId, 10),
        IsComplete: document.getElementById('edit-completion').checked,
        Name: document.getElementById('edit-name').value.trim() ?? null,
        Deadline: document.getElementById('edit-deadline').value.trim() || null,
        Description: document.getElementById('edit-desc').value.trim() || null,
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PATCH',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount() {
    fetch(`${uri}/Count`)
        .then(response => response.json())
        .then(data => {
            const name = (data === '1') ? 'to-do' : 'to-dos';
            document.getElementById('counter').innerText = `${data} ${name}`;
        })
        .catch(error => console.error('Unable to get items.', error));
}

function _displayItems(data) {
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(item => {
        let isCompleteCheckbox = document.createElement('input');
        isCompleteCheckbox.type = 'checkbox';
        isCompleteCheckbox.disabled = true;
        isCompleteCheckbox.checked = item.isComplete;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isCompleteCheckbox);

        let td2 = tr.insertCell(1);
        let nameNode = document.createTextNode(item.name);
        td2.appendChild(nameNode);

        let td3 = tr.insertCell(2);
        let descNode = document.createTextNode(item.description || '');
        td3.appendChild(descNode);

        let td4 = tr.insertCell(3);
        let createdNode = document.createTextNode(item.creation || '');
        td4.appendChild(createdNode);

        let td5 = tr.insertCell(4);
        let deadlineNode = document.createTextNode(item.deadline || '');
        td5.appendChild(deadlineNode);

        let td6 = tr.insertCell(5);
        td6.appendChild(editButton);

        let td7 = tr.insertCell(6);
        td7.appendChild(deleteButton);
    });

    todos = data;
}