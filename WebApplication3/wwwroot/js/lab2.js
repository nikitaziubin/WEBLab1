const uri = 'api/rooms';
const uriRoomtypes = 'api/roomTypes';
let rooms = [];


fetch(uriRoomtypes)
    .then(response => response.json()) 
    .then(roomTypes => { 
        roomTypes.forEach(roomType => { 
            const optionElement = document.createElement("option"); 
            optionElement.value = roomType.id;
            optionElement.text = roomType.roomType;
            selectElement.appendChild(optionElement);
        });
    })
    .catch(error => console.error(error));
const selectElement = document.getElementById("mySelect");


function getRooms() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayRooms(data))
        .catch(error => console.error('Unable to get categories.', error));
}

function addCategory() {
    const addroomNumberTextbox = document.getElementById('add-roomNumber');
    const addoneNightPriceTextbox = document.getElementById('add-oneNightPrice');
    const addstateTextbox = document.getElementById('add-state');
    const addroomTypeTextbox = document.getElementById('mySelect');
    const category = {
        roomNumber: addroomNumberTextbox.value.trim(),
        oneNightPrice: addoneNightPriceTextbox.value.trim(),
        state: addstateTextbox.value.trim(),
        roomTypeId: addroomTypeTextbox.value.trim(),
    };
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(category)
    })
        .then(response => response.json())
        .then(() => {
            getRooms();
        })
        .catch(error => console.error('Unable to add category.', error));
}

function deleteCategory(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getRooms())
        .catch(error => console.error('Unable to delete category.', error));
}

function displayEditForm(id) {
    fetch(uriRoomtypes)
        .then(response => response.json())
        .then(roomTypes => {
            roomTypes.forEach(roomType => {
                const optionElement = document.createElement("option");
                optionElement.value = roomType.id;
                optionElement.text = roomType.roomType;
                selectElement.appendChild(optionElement);
            });
        })
        .catch(error => console.error(error));
    const selectElement = document.getElementById("edit-mySelect");
    const room = rooms.find(room => room.id === id);
    document.getElementById('edit-id').value = room.id;
    document.getElementById('edit-roomNumber').value = room.roomNumber;
    document.getElementById('edit-oneNightPrice').value = room.oneNightPrice;
    document.getElementById('edit-state').value = room.state;
    document.getElementById('edit-mySelect');// = room.roomTypeid;
    document.getElementById('editForm').style.display = 'block';
}

function updateCategory() {
    const categoryId = document.getElementById('edit-id').value;
    const room = {
        id: document.getElementById('edit-id').value.trim(),
        roomNumber: document.getElementById('edit-roomNumber').value.trim(),
        oneNightPrice: document.getElementById('edit-oneNightPrice').value.trim(),
        state: document.getElementById('edit-state').value.trim(),
        roomTypeid: document.getElementById('edit-mySelect').value.trim()
    };
    fetch(`${uri}/${categoryId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(room)
    })
        .then(() => getRooms())
        .catch(error => console.error('Unable to update category.', error));
    closeInput();
    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayRooms(data) {
    const tBody = document.getElementById('rooms');

    tBody.innerHTML = '';
    const button = document.createElement('button');
    data.forEach(room => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${room.id})`);
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteCategory(${room.id})`);
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(room.roomNumber);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNodeInfo = document.createTextNode(room.oneNightPrice);
        td2.appendChild(textNodeInfo);

        let td = tr.insertCell(2);
        let t = document.createTextNode(room.roomTypeNavigation.roomType);
        td.appendChild(t);

        let td3 = tr.insertCell(3);
        let text = document.createTextNode(room.state);
        td3.appendChild(text);

        let td4 = tr.insertCell(4);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(5);
        td5.appendChild(deleteButton);
    });
    rooms = data;
}
