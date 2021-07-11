import eventsReducer from "./events-reducer";
import {applyMiddleware, combineReducers, createStore} from "redux";
import eventAreasReducer from "./eventAreas-reducer";
import authReducer from "./auth-reducer";
import thunkMiddleware from "redux-thunk";
import {reducer as formReducer} from 'redux-form';
import basketReducer from "./basket-reducer";
import purchaseHistoryReducer from "./purchaseHisory-reducer";
import profileReducer from "./profile-reducer";
import eventManagerAreaReducer from "./eventManagerArea-reducer";

let reducers =  combineReducers(
    {
        eventsPage: eventsReducer,
        eventAreasPage: eventAreasReducer,
        authPage: authReducer,
        basketPage: basketReducer,
        purchaseHistoryPage: purchaseHistoryReducer,
        profilePage: profileReducer,
        eventManagerAreaPage: eventManagerAreaReducer,
        form : formReducer
    }
)

let store = createStore(reducers, applyMiddleware(thunkMiddleware));

window.store = store;

export default store;