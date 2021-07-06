import eventsReducer from "./events-reducer";
import {applyMiddleware, combineReducers, createStore} from "redux";
import eventAreasReducer from "./eventAreas-reducer";
import authReducer from "./auth-reducer";
import thunkMiddleware from "redux-thunk";
import {reducer as formReducer} from 'redux-form';

let reducers =  combineReducers(
    {
        eventsPage: eventsReducer,
        eventAreasPage: eventAreasReducer,
        authPage: authReducer,
        form : formReducer
    }
)

let store = createStore(reducers, applyMiddleware(thunkMiddleware));

window.store = store;

export default store;