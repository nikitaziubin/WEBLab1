const uri = 'api/rooms';
const uriRoomtypes = 'api/roomTypes';
const uriBoking = 'api/bookings';
const uriClient = 'api/clients';
let rooms = [];
let booking;
let currentPage = 1;
const rowsPerPage = 10;

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




function addCategory() {
    const addroomNumberTextbox = document.getElementById('add-roomNumber');
    const addoneNightPriceTextbox = document.getElementById('add-oneNightPrice');
    const addroomTypeTextbox = document.getElementById('mySelect');
    const category = {
        roomNumber: addroomNumberTextbox.value.trim(),
        oneNightPrice: addoneNightPriceTextbox.value.trim(),
        state: false, /*addstateTextbox.value.trim(),*/
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
    const roomID = document.getElementById('edit-id').value;
    const room = {
        id: document.getElementById('edit-id').value.trim(),
        roomNumber: document.getElementById('edit-roomNumber').value.trim(),
        oneNightPrice: document.getElementById('edit-oneNightPrice').value.trim(),
        state: document.getElementById('edit-state').value.trim(),
        roomTypeid: document.getElementById('edit-mySelect').value.trim()
    };
    fetch(`${uri}/${roomID}`, {
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

function redirectToBookingPage(roomId) {
    window.location.href = `Booking.html?room=${roomId}`;
}

function setBooking(data, tr) {
    booking = data;
    let td7 = tr.insertCell(7);
    textNode = document.createTextNode(booking.clientNavigation.name);
    td7.appendChild(textNode);

    let td8 = tr.insertCell(8);
    textNode = document.createTextNode(booking.arrivalDate.split("T")[0]);
    td8.appendChild(textNode);

    let td9 = tr.insertCell(9);
    textNode = document.createTextNode(booking.departureDate.split("T")[0]);
    td9.appendChild(textNode);
}

function getBookingByRoomId(roomId, tr) {
    fetch(`${uriBoking}/${roomId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => setBooking(data, tr))
        .catch(error => console.error('Unable to update category.', error));
}

function getRooms() {
    fetch(uri)
        .then(response => response.json())
        .then(data => {
            rooms = data;
            displayRoomsOnCurrentPage();
            displayPaginationButtons();
        })
        /*_displayRooms(data))*/
        .catch(error => console.error('Unable to get categories.', error));
}

function displayRoomsOnCurrentPage() {
    const start = (currentPage - 1) * rowsPerPage;
    const end = start + rowsPerPage;
    _displayRooms(rooms.slice(start, end));
}

function displayPaginationButtons() {
    const paginationContainer = document.querySelector('.pagination');
    paginationContainer.innerHTML = '';

    const ul = document.createElement('ul');
    ul.className = 'paginationList';


    const pageCount = Math.ceil(rooms.length / rowsPerPage);
    for (let i = 1; i <= pageCount; i++) {
        const li = document.createElement('li');
        li.className = 'paginationItem';


        const button = document.createElement('button');
        button.innerText = i;
        button.className = i === currentPage ? 'btn btn-outline-light btn-sm active' : 'btn btn-outline-light btn-sm';
        button.addEventListener('click', () => {
            currentPage = i;
            displayRoomsOnCurrentPage();
            displayPaginationButtons();
        });

        li.appendChild(button);
        ul.appendChild(li);

        paginationContainer.appendChild(button);
    }
}


function _displayRooms(data) {
    const tBody = document.getElementById('rooms');

    tBody.innerHTML = '';
    const button = document.createElement('button');
    data.forEach(room => {

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${room.id})`);
        editButton.classList.add('btn', 'btn-light');

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteCategory(${room.id})`);
        deleteButton.classList.add('btn', 'btn-light');

        let makeBooking = button.cloneNode(false);
        makeBooking.innerText = 'Booking';
        makeBooking.setAttribute('onclick', `redirectToBookingPage(${room.id})`);
        makeBooking.classList.add('btn', 'btn-light');

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

        if (room.state == false) {

            let td6 = tr.insertCell(6);
            td6.appendChild(makeBooking);

            let td7 = tr.insertCell(7);
            let textNode = document.createTextNode('None');
            td7.appendChild(textNode);

            let td8 = tr.insertCell(8);
            textNode = document.createTextNode('None');
            td8.appendChild(textNode);

            let td9 = tr.insertCell(9);
            textNode = document.createTextNode('None');
            td9.appendChild(textNode);
        }
        else {
            let td6 = tr.insertCell(6);
            let textNode = document.createTextNode('Boked');
            td6.appendChild(textNode);
            getBookingByRoomId(room.id, tr);
        }
    });
    /*rooms = data;*/
}
