import {basketAPI, profileAPI, purchaseHistoryAPI} from "../API/api";
import {userClaim} from "../components/Constants/userConst";
import {setEvents} from "./events-reducer";

const TOGGLE_IS_FETCHING = 'TOGGLE_IS_FETCHING';
const SET_ITEMS = 'SET_ITEMS';
const SET_TOTAL_PRICE = 'SET_TOTAL_PRICE';
const SET_BALANCE = 'SET_BALANCE';
const SET_USER_ID = 'SET_SET_USER_ID';
const set_isNotEnoughMoney = 'set_isNotEnoughMoney';

let initialState = {
    id: null,
    totalPrice: null,
    balance: null,
    isFetching: false,
    isNotEnoughMoney: false,
    items: []
}

const basketReducer = (state = initialState, action) => {
    switch (action.type) {
        case SET_USER_ID: {
            return {...state, id: action.id}
        }
        case set_isNotEnoughMoney: {
            return {...state, isNotEnoughMoney: action.isNotEnoughMoney}
        }
        case TOGGLE_IS_FETCHING: {
            return {...state, isFetching: action.isFetching}
        }
        case SET_ITEMS: {
            return {
                ...state,
                items: action.items,
                isNotEnoughMoney: false
            }
        }
        case SET_TOTAL_PRICE: {
            return {...state, totalPrice: action.totalPrice}
        }
        case SET_BALANCE: {
            return {...state, balance: action.balance}
        }
        default:
            return state;
    }
}

export const toggleIsFetching = (isFetching) => ({type: TOGGLE_IS_FETCHING, isFetching})
export const setItems = (items) => ({type: SET_ITEMS, items})
export const setTotalPrice = (totalPrice) => ({type: SET_TOTAL_PRICE, totalPrice})
export const setBalance = (balance) => ({type: SET_BALANCE, balance})
export const setUserId = (id) => ({type: SET_USER_ID, id})
export const setIsNotEnoughMoney = (isNotEnoughMoney) => ({type: set_isNotEnoughMoney, isNotEnoughMoney})

const userId = () => {
    const localUser = localStorage.getItem('user');
    const parsedUser = JSON.parse(localUser);
    let userCredential = (parsedUser) => ({
        id: parsedUser[userClaim.id],
        name: parsedUser[userClaim.name],
        email: parsedUser[userClaim.email],
        role: parsedUser[userClaim.role],
    });
    let id = userCredential(parsedUser).id;

    return id;
}

export const getUserItems = () => (dispatch) => {
    const id = userId();
    basketAPI.getUserItems(id)
        .then(response => {
            dispatch(toggleIsFetching(false));
            dispatch(setItems(response.data.items));
            dispatch(setTotalPrice(response.data.totalPrice));
        })
}

export const getBalance = () => (dispatch) => {
    const id = userId();
    profileAPI.getBalance(id)
        .then(response => {
            dispatch(setBalance(response.data))
        })
}

export const buy = (totalPrice, balance, items) => (dispatch) => {
    const id = userId();
    debugger;
    {
        (balance >= totalPrice) ?
            profileAPI.updateBalance(id, balance - totalPrice)
                .then(response => {
                    items.map(i =>
                        purchaseHistoryAPI.AddItem(id, i.id)
                            .then(response => {
                                debugger;
                            }))
                    basketAPI.deleteAllItemsFromUserBasket(id)
                })
            : dispatch(setIsNotEnoughMoney(true))
    }
    debugger;
}

export default basketReducer;
