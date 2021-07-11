import * as axios from "axios";

const identityBaseURL = 'https://localhost:44370/';
const eventBaseURL = 'https://localhost:44300/';

const identityInstance = axios.create({
    baseURL: identityBaseURL
});
const eventInstance = axios.create({
    baseURL: eventBaseURL
});

const AuthStr = 'Bearer '.concat(localStorage.getItem('token'));
const headerWithAuthStr = {headers: {Authorization: AuthStr}};

export const authAPI = {
    validateToken(token = "") {
        return identityInstance.get('AccountUser?token=' + token)
    },
    login(email, password, rememberMe = false) {
        return identityInstance.post('AccountUser/login', {email, password, rememberMe});
    },
    register(userName, firstName, surname, email, password, passwordConfirm, language = "asd", timeZoneOffset = "ads") {
        return identityInstance.post('AccountUser/',
            {userName, firstName, surname, language, timeZoneOffset, email, password, passwordConfirm});
    }
}

export const eventAPI = {
    getEvents() {
        return eventInstance.get('Event')
    },
    deleteEventById(id) {
        return eventInstance.delete(`Event?id=${id}`, headerWithAuthStr)
    },
    updateEvent(event) {
        return eventInstance.put(`Event`, event, headerWithAuthStr)
    },
    getLastEvent() {
        return eventInstance.get('/Event/get-last', headerWithAuthStr)
    },
    getEventById(id) {
        return eventInstance.get(`/Event/get-by-id?id=${id}`, headerWithAuthStr)
    },
}
export const eventManagerAPI = {
    createEvent(event) {
        return eventInstance.post('EventManager', event, headerWithAuthStr)
    },
}

export const eventAreaAPI = {
    getEventAreas(eventId) {
        return eventInstance.get(`EventArea?id=${eventId}`, headerWithAuthStr)
    },
    updatePrices(id, price) {
        let eventAreas = [{
            id: id,
            price: price
        }]
        debugger;
        return eventInstance.put("EventArea", eventAreas, headerWithAuthStr)
    },
}
export const profileAPI = {
    getProfile(id = "") {
        return identityInstance.get(`/Profile?userId=${id}`, headerWithAuthStr)
    },
    getBalance(id) {
        return identityInstance.get(`/Profile/getBalance?userId=${id}`, headerWithAuthStr)
    },
    updateBalance(userId, balance) {
        return identityInstance.put("Profile", {userId, balance}, headerWithAuthStr)
    },
    editFirstName(userId, firstName) {
        return identityInstance.put("/Profile/edit-first-name", {userId, firstName}, headerWithAuthStr)
    },
    editSurname(userId, surname) {
        return identityInstance.put("/Profile/edit-surname", {userId, surname}, headerWithAuthStr)
    },
    editEmail(userId, email) {
        return identityInstance.put("/Profile/edit-email", {userId, email}, headerWithAuthStr)
    },
    editPassword(userId, oldPassword, newPassword) {
        return identityInstance.put("/Profile/edit-password", {userId, oldPassword, newPassword}, headerWithAuthStr)
    },
    editTimeZoneOffset(userId, timeZoneOffset) {
        return identityInstance.put("/Profile/edit-time-zone-offset", {userId, timeZoneOffset}, headerWithAuthStr)
    },
    deposit(userId, balance) {
        return identityInstance.put("/Profile/deposit", {userId, balance}, headerWithAuthStr)
    },
    setLanguage(userId, culture) {
        return identityInstance.put("/Profile/set-language", {userId, culture}, headerWithAuthStr)
    },
}

export const purchaseHistoryAPI = {
    getUserItems(id = "") {
        return eventInstance.get(`PurchaseHistory?userId=${id}`, headerWithAuthStr)
    },
    AddItem(userId = "", itemId = "0") {
        return eventInstance.post('/PurchaseHistory', {userId, itemId}, headerWithAuthStr)
    },
}

export const basketAPI = {
    getUserItems(id = "") {
        return eventInstance.get(`Basket?id=${id}`, headerWithAuthStr)
    },
    addItemToUserBasket(userId = "", itemId = "0") {
        return eventInstance.post('Basket', {userId, itemId}, headerWithAuthStr)
    },
    deleteItemFromUserBasket(userId = "", itemId = "0") {
        return eventInstance.delete(`Basket?userId=${userId}&itemId=${itemId}`, headerWithAuthStr)
    },
    deleteAllItemsFromUserBasket(userId = "") {
        return eventInstance.delete(`Basket/delete-all-by-user-id?id=${userId}`, headerWithAuthStr)
    },
}