import {eventAPI, eventAreaAPI, eventManagerAPI} from "../API/api";

const TOGGLE_IS_FETCHING = 'TOGGLE_IS_FETCHING';
const SET_EVENTS = 'SET_EVENTS';
const SET_EVENT = 'SET_EVENT';
const SET_IN_PROGRESS = 'SET_IN_PROGRESS';
const SET_EVENT_IN_PROGRESS = 'SET_EVENT_IN_PROGRESS';
const SET_IS_CREATE_EVENT_SUCCESSFUL = 'SET_IS_CREATE_EVENT_SUCCESSFUL';
const SET_IS_UPDATE_EVENT_SUCCESSFUL = 'SET_IS_UPDATE_EVENT_SUCCESSFUL';
let initialState = {
    id: null,
    isFetching: false,
    events: [],
    inProgress: false,
    event: null,
    isCreateEventSuccessful: false,
    isUpdateEventSuccessful: false

}

const eventManagerAreaReducer = (state = initialState, action) => {
    switch (action.type) {
        case TOGGLE_IS_FETCHING: {
            return {...state, isFetching: action.isFetching}
        }
        case SET_IS_CREATE_EVENT_SUCCESSFUL: {
            return {...state, isCreateEventSuccessful: action.isCreateEventSuccessful}
        }
        case SET_IS_UPDATE_EVENT_SUCCESSFUL: {
            return {...state, isUpdateEventSuccessful: action.isUpdateEventSuccessful}
        }
        case SET_IN_PROGRESS: {
            return {...state, inProgress: action.inProgress}
        }
        case SET_EVENT_IN_PROGRESS: {
            return {...state, creteEventInProgress: action.creteEventInProgress,
            isFetching: false}
        }
        case SET_EVENTS: {
            return {...state, events: action.events}
        }
        case SET_EVENT: {
            return {...state, event: action.event}
        }


        default:
            return state;
    }
}

export const setIsUpdateEventSuccessful = (isUpdateEventSuccessful) => ({type: SET_IS_UPDATE_EVENT_SUCCESSFUL, isUpdateEventSuccessful})
export const setIsCreateEventSuccessful = (isCreateEventSuccessful) => ({type: SET_IS_CREATE_EVENT_SUCCESSFUL, isCreateEventSuccessful})
export const setEventInProgress = (creteEventInProgress) => ({type: SET_EVENT_IN_PROGRESS, creteEventInProgress})
export const setInProgress = (inProgress) => ({type: SET_IN_PROGRESS, inProgress})
export const toggleIsFetching = (isFetching) => ({type: TOGGLE_IS_FETCHING, isFetching})
export const setEvents = (events) => ({type: SET_EVENTS, events})
export const setEvent = (event) => ({type: SET_EVENT, event})

export const getEvent = (id) => (dispatch) => {
    debugger;
    dispatch(toggleIsFetching(true));
    eventAPI.getEventById(id)
        .then(response => {
            debugger;
            dispatch(setEvent(response.data));
            dispatch(toggleIsFetching(false));
        })
}

export const updateEvent = (event) => (dispatch) => {
    debugger;
    dispatch(toggleIsFetching(true));
    eventAPI.updateEvent(event)
        .then(response => {
            debugger;
            dispatch(setEvent(response.data));
            dispatch(toggleIsFetching(false));
        })
}

export const getEvents = () => (dispatch) => {
    dispatch(toggleIsFetching(true));
    eventAPI.getEvents()
        .then(response => {
            dispatch(setEvents(response.data));
            dispatch(toggleIsFetching(false));
        })
}

export const createEvent = (event) => (dispatch) => {
    let createdEvent = null;
    dispatch(setInProgress(true));
    if (!event.eventAreas) {
        eventManagerAPI.createEvent(event)
            .then(() => {
                eventAPI.getLastEvent()
                    .then(response => {
                        createdEvent = response.data;
                        eventAreaAPI.getEventAreas(createdEvent.id)
                            .then(response => {
                                createdEvent.eventAreas = response.data;
                                dispatch(setEvent(createdEvent));
                                dispatch(setInProgress(false));
                            })
                    })
            })
    }
    if (event.eventAreas) {
        event.eventAreas.map((ea, index) => {
                eventAreaAPI.updatePrices(index, ea.price)
            }
        )
        dispatch(setEventInProgress(false));
        dispatch(setInProgress(false));
        dispatch(setEvent(null));
        dispatch(setIsCreateEventSuccessful(true));
    }

}

export default eventManagerAreaReducer;
