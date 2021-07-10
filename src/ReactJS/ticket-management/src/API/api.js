import * as axios from "axios";

const identityBaseURL = 'https://localhost:5004/';
const eventBaseURL = 'https://localhost:5003/';

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
}

export const eventAreaAPI = {
    getEventAreas(eventId) {
        return eventInstance.get(`EventArea?${eventId}`, headerWithAuthStr)
    },
}
export const profileAPI = {
    getProfile(id = "") {
        return eventInstance.get(`/Profile?id=${id}`, headerWithAuthStr)
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
    editPassword(userId, password) {
        return identityInstance.put("/Profile/edit-password", {userId, password}, headerWithAuthStr)
    },
    editTimeZoneOffset(userId, timeZoneOffset) {
        return identityInstance.put("/Profile/edit-time-zone-offset", {userId, timeZoneOffset}, headerWithAuthStr)
    },
    deposite(userId, balance) {
        return identityInstance.put("/Profile/deposit", {userId, balance}, headerWithAuthStr)
    },
    setLanguage(userId, culture) {
        return identityInstance.put("/Profile/set-language", {userId, culture}, headerWithAuthStr)
    },
}

export const purchaseHistoryAPI = {
    getUserItems(id= "") {
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