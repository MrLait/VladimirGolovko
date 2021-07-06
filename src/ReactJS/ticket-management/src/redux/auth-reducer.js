import {authAPI} from "../API/api";
import jwtDecode from "jwt-decode";
import React from "react";
import {stopSubmit} from "redux-form";

const SET_USER_DATA = 'SET_USER_DATA';
const IS_TOKEN_VALID = 'IS_TOKEN_VALID';
const SET_TOKEN = 'SET_TOKEN';

let initialState = {
    id: null,
    name: null,
    email: null,
    role: null,
    token: null,
    isAuth: false,
    isTokenValid: false
}

const authReducer = (state = initialState, action) => {
    switch (action.type) {
        case SET_TOKEN: {
            return {
                ...state,
                token: action.token,
            }
        }
        case SET_USER_DATA: {
            return {
                ...state,
                ...action.payload,
            }
        }
        case IS_TOKEN_VALID: {
            return {
                ...state,
                isTokenValid: action.isTokenValid,
            }
        }
        default:
            return state;
    }
}
export const setToken = (token) => ({type: SET_TOKEN, token})
export const setAuthData = (id, name, email, role, isAuth) => ({
    type: SET_USER_DATA,
    payload: {id, name, email, role, isAuth}
})
export const isTokenValid = (isTokenValid) => ({type: IS_TOKEN_VALID, isTokenValid})

const setUserLocalStorage = (token) => {
    const jwtData = JSON.stringify(jwtDecode(token));
    localStorage.setItem('user', jwtData);
}
const setTokenToLocalStorage = (token) => {
    localStorage.setItem('token', token);
}

let userCredential = (parsedUser) => ({
    id: parsedUser["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
    name: parsedUser["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
    email: parsedUser["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
    role: parsedUser["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
});

export const initAuthDataFromLocalStorage = (token) => (dispatch) => {
    if (!token) {
        const localToken = localStorage.getItem('token');
        if (localToken) {
            authAPI.validateToken(localToken)
                .then(response => {
                    if (response.status = "200") {
                        dispatch(isTokenValid(true));
                        setUserLocalStorage(localToken);
                        const localUser = localStorage.getItem('user');
                        const parsedUser = JSON.parse(localUser);
                        let user = userCredential(parsedUser);
                        dispatch(setToken(localToken));
                        dispatch(setAuthData(user.id, user.name, user.email, user.role, true));
                    }
                });
        }
    }
}

export const login = (email, password, rememberMe) => (dispatch) => {
    authAPI.login(email, password, rememberMe)
        .then(response => {
            if (response.data) {
                let token = response.data;
                dispatch(setToken(token));
                setTokenToLocalStorage(token);
                setUserLocalStorage(token);
                const localUser = localStorage.getItem('user');
                const parsedUser = JSON.parse(localUser);
                let user = userCredential(parsedUser);
                dispatch(setAuthData(user.id, user.name, user.email, user.role, true));
            } else {
                debugger;
                dispatch(stopSubmit('login', {_error: response.data}));
            }
        }).catch(error =>  {
            if(!error.response.request.Succeeded)
            {
                dispatch(stopSubmit('login', {_error: "Incorrect username and(or) password"}));
            }

        console.log(error);
    });
}

export const logout = () => (dispatch) => {
    dispatch(setToken(null, null, null, null));
    dispatch(setAuthData(null));
    localStorage.clear();
}
export default authReducer;
