const uri = 'api/rooms';
const uriClient = 'api/clients';
const uriRoomtypes = 'api/roomTypes';
const uriBoking = 'api/bookings';
let clients = [];
let rooms = [];

function getbooking() {
    Promise.all([fetch(uri), fetch(uriClient)])
        .then(responses => {
            const [response1, response2] = responses;
            return Promise.all([response1.json(), response2.json()]);
        })
        .then(data => {
            const [data1, data2] = data;
            rooms = data1;
            clients = data2;
            displayBooking(data1, data2);

        })
        .catch(error => console.error('Error:', error));
}

function displayBooking(datarooms, dataclients) {

    const queryString = window.location.search;
    const searchParams = new URLSearchParams(queryString);
    const roomId = parseInt(searchParams.get('room'));
    const room = datarooms.find(room => room.id === roomId);
    const client = dataclients.find(client => client.id === 1);
    document.getElementById('roomNumber').value = room.roomNumber;
    document.getElementById('OneKnightPrice').value = room.oneNightPrice;
    document.getElementById('Name').value = client.name;
    //document.getElementById('edit-roomNumber').value = room.roomNumber;

}

function addBooking() {
    const roomNumber = parseInt(document.getElementById('roomNumber').value);
    const roomId = rooms.find(roomId => roomId.roomNumber === roomNumber);

    const arrivalDate = document.getElementById('add-Arrival-date');
    const departureDate = document.getElementById('add-Departure-date');
    const totalPrice = document.getElementById('add-All-price');
    const clientId = document.getElementById('Name');
    const booking = {
        roomId: roomId.id,
        arrivalDate: arrivalDate.value.trim(),
        departureDate: departureDate.value.trim(),
        totalPrice: totalPrice.value.trim(),
        clientId: 1,
        roomNavigation: roomId,
    };

    fetch(uriBoking, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(booking)
    })
        .then(response => response.json())
        .then(() => {
            window.location.href = '/Index.html';
        })
        .catch(error => console.error('Unable to add category.', error));
    //a(roomNumber);
    
}
function PUTRoom(roomNumber) {
    /*const id = roomId.id;*/
    const roomId = rooms.find(roomId => roomId.roomNumber === roomNumber);
    const room = {
        id: roomId.id,
        roomNumber: roomId.roomNumber,
        oneNightPrice: roomId.oneNightPrice,
        state: true,
        roomTypeId: roomId.roomTypeNavigation.roomType,
    };
    //let roomID = String(room.id);
    //fetch(`${uri}/${roomID}`, {
    //    method: 'PUT',
    //    headers: {
    //        'Accept': 'application/json',
    //        'Content-Type': 'application/json'
    //    },
    //    body: JSON.stringify(room)
    //})
    //    .then(() => getRooms())
    //    .catch(error => console.error('Unable to update category.', error));
}

function calculateSum() {
    const arrivalDate = document.getElementById('add-Arrival-date').value;
    const departureDate = document.getElementById('add-Departure-date').value;
    const startDate = new Date(arrivalDate);
    const endDate = new Date(departureDate);
    const diff = endDate.getTime() - startDate.getTime();
    const diffDays = Math.floor(diff / (1000 * 60 * 60 * 24));
    const oneNightPrice = document.getElementById('OneKnightPrice').value;

    document.getElementById('add-All-price').value = oneNightPrice * diffDays;
    document.getElementById('add-Arrival-date').value = arrivalDate;
    document.getElementById('add-Departure-date').value = departureDate;
}
