import eventsReducer from "./events-reducer";
import {combineReducers, createStore} from "redux";
import eventAreasReducer from "./eventAreas-reducer";
import authReducer from "./auth-reducer";

let reducers =  combineReducers(
    {
        eventsPage: eventsReducer,
        eventAreasPage: eventAreasReducer,
        authPage: authReducer
    }
)

let store = createStore(reducers);

window.store = store;

export default store;