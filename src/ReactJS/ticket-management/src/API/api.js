import * as axios from "axios";

const accountInstance = axios.create({
    baseURL: 'https://localhost:5004/'
});

const basketInstance = axios.create({
    baseURL: 'https://localhost:5003/'
});

const AuthStr = 'Bearer '.concat(localStorage.getItem('token'));
const headerWithAuthStr = {headers: { Authorization: AuthStr }};

export const authAPI = {
    validateToken(token = "") {
        return accountInstance.get('AccountUser?token=' + token)
    },
    login(email, password, rememberMe = false) {
        return accountInstance.post('AccountUser/login', {email, password, rememberMe});
    },
    register(userName, firstName, surname, email, password, passwordConfirm, language = "asd", timeZoneOffset = "ads") {
        return accountInstance.post('AccountUser/', {userName, firstName, surname,language, timeZoneOffset, email, password, passwordConfirm});
    }
}

export const basketAPI = {
    getUserItems(id = "") {
        return basketInstance.get('Basket?id' + id, headerWithAuthStr)
    },
    addItemToUserBasket(userId = "", itemId = "0") {
        return basketInstance.post('Basket', {userId,itemId}, headerWithAuthStr)
    },
    deleteItemFromUserBasket(userId = "", itemId = "0") {
        return basketInstance.delete('Basket', {userId,itemId}, headerWithAuthStr)
    },
    deleteAllItemsFromUserBasket(userId = "", itemId = "0") {
        return basketInstance.delete('Basket/delete-all-by-user-id', {userId, itemId}, headerWithAuthStr)
    },
}