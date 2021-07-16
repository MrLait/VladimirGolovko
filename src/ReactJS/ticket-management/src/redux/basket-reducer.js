import {basketAPI, profileAPI, purchaseHistoryAPI} from "../API/api";
import {getCurUserId} from "../common/Services/UserService";

const TOGGLE_IS_FETCHING = 'TOGGLE_IS_FETCHING';
const SET_ITEMS = 'SET_ITEMS';
const SET_TOTAL_PRICE = 'SET_TOTAL_PRICE';
const SET_BALANCE = 'SET_BALANCE';
const SET_USER_ID = 'SET_SET_USER_ID';
const SET_IS_NOT_ENOUGH_MONEY = 'SET_IS_NOT_ENOUGH_MONEY';
const SET_IN_PROGRESS = 'SET_IN_PROGRESS';
const SET_IS_BUY_SUCCESSFUL = 'SET_IS_BUY_SUCCESSFUL';
let initialState = {
    id: null,
    totalPrice: null,
    balance: null,
    isFetching: false,
    isNotEnoughMoney: false,
    items: [],
    isBuySuccessful: false,
    inProgress: false,
}

const basketReducer = (state = initialState, action) => {
    switch (action.type) {
        case SET_USER_ID: {
            return {...state, id: action.id}
        }
        case SET_IS_NOT_ENOUGH_MONEY: {
            return {...state, isNotEnoughMoney: action.isNotEnoughMoney}
        }
        case TOGGLE_IS_FETCHING: {
            return {...state, isFetching: action.isFetching}
        }
        case SET_IS_BUY_SUCCESSFUL: {
            return {
                ...state,
                isBuySuccessful: action.isBuySuccessful
            }
        }
        case SET_IN_PROGRESS: {
            return {...state, inProgress: action.inProgress}
        }
        case SET_ITEMS: {
            return {
                ...state,
                items: action.items,
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

export const setIsBuySuccessful = (isBuySuccessful) => ({type: SET_IS_BUY_SUCCESSFUL, isBuySuccessful})
export const setInProgress = (inProgress) => ({type: SET_IN_PROGRESS, inProgress})
export const toggleIsFetching = (isFetching) => ({type: TOGGLE_IS_FETCHING, isFetching})
export const setItems = (items) => ({type: SET_ITEMS, items})
export const setTotalPrice = (totalPrice) => ({type: SET_TOTAL_PRICE, totalPrice})
export const setBalance = (balance) => ({type: SET_BALANCE, balance})
export const setUserId = (id) => ({type: SET_USER_ID, id})
export const setIsNotEnoughMoney = (isNotEnoughMoney) => ({type: SET_IS_NOT_ENOUGH_MONEY, isNotEnoughMoney})

export const getUserItems = () => (dispatch) => {
    const id = getCurUserId();
    basketAPI.getUserItems(id)
        .then(response => {
            dispatch(toggleIsFetching(false));
            dispatch(setItems(response.data.items));
            dispatch(setTotalPrice(response.data.totalPrice));
        })
}

export const getBalance = () => (dispatch) => {
    const id = getCurUserId();
    profileAPI.getBalance(id)
        .then(response => {
            dispatch(setBalance(response.data))
        })
}

export const buy = (totalPrice, balance, items) => (dispatch) => {
    const id = getCurUserId();
    dispatch(setInProgress(true));
    dispatch(setIsBuySuccessful(false));
    {
        if (balance >= totalPrice) {
            profileAPI.updateBalance(id, balance - totalPrice)
                .then(() => {
                    items.map(i => purchaseHistoryAPI.AddItem(id, i.id))
                    basketAPI.deleteAllItemsFromUserBasket(id).then(() => {
                        dispatch(setInProgress(false));
                        dispatch(setIsBuySuccessful(true));
                        dispatch(setItems([]));
                    })
                })
        } else
            dispatch(setIsNotEnoughMoney(true))
        dispatch(setInProgress(false))
    }
}

export default basketReducer;
