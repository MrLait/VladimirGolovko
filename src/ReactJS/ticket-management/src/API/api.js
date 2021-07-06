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
    }
}