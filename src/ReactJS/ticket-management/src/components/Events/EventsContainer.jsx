import React from "react";
import {connect} from "react-redux";
import {setEvents, toggleIsFetching} from "../../redux/events-reducer";
import * as axios from "axios";
import Events from "./Events";
import Preloader from "../../common/Preloaders/Preloader";

class EventsContainer extends React.Component {
    componentDidMount() {
        this.props.toggleIsFetching(true);
        axios.get("https://localhost:5003/Event").then(response => {
            this.props.toggleIsFetching(false);
            this.props.setEvents(response.data);
        })
    }
    render() {
        return <>
            {this.props.isFetching ? <Preloader /> : null}
            <Events {...this.props}/>
        </>
    }
}

let mapStateToProps = (state) => {
    return {
        events: state.eventsPage.events,
        isFetching: state.eventsPage.isFetching,
        isAuth: state.authPage.isAuth
    }
}

export default connect(
    mapStateToProps,
    {
        setEvents: setEvents,
        toggleIsFetching: toggleIsFetching
        }) (EventsContainer);