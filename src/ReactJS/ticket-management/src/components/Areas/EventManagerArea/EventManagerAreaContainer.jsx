import React from "react";
import {connect} from "react-redux";
import Preloader from "../../../common/Preloaders/Preloader";
import EventManagerArea from "./EventManagerArea";
import {getEvents} from "../../../redux/eventManagerArea-reducer";

class EventManagerAreaContainer extends React.Component {
    componentDidMount() {
        this.props.getEvents();
    }
    render() {
        return <>
            {this.props.isFetching ? <Preloader /> : <EventManagerArea {...this.props}/>}
        </>
    }
}

let mapStateToProps = (state) => {
    return {
        isAuth: state.authPage.isAuth,
        events:state.eventManagerAreaPage.events,
        isFetching:state.eventManagerAreaPage.isFetching,
    }
}

export default connect(
    mapStateToProps,
    {
        getEvents
        }) (EventManagerAreaContainer);