import React from "react";
import {connect} from "react-redux";
import {getEvents, toggleIsFetching} from "../../redux/events-reducer";
import Events from "./Events";
import Preloader from "../../common/Preloaders/Preloader";

class EventsContainer extends React.Component {
    componentDidMount() {
        this.props.getEvents();
    }
    render() {
        return <>
            {this.props.isFetching ? <Preloader /> : <Events {...this.props}/>}

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
        toggleIsFetching, getEvents
        }) (EventsContainer);