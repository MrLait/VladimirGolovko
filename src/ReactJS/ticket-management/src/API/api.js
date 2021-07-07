import * as axios from "axios";

const instance = axios.create({
    baseURL: 'https://localhost:5004/'
});

export const authAPI = {
    validateToken(token = "") {
        return instance.get('AccountUser?token=' + token)
    },
    login(email, password, rememberMe = false) {
        return instance.post('AccountUser/login', {email, password, rememberMe});
    },
    register(userName, firstName, surname, email, password, passwordConfirm, language = "asd", timeZoneOffset = "ads") {
        return instance.post('AccountUser/', {userName, firstName, surname,language, timeZoneOffset, email, password, passwordConfirm});
    }
}