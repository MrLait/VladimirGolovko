import React from "react";
import {connect} from "react-redux";
import Preloader from "../../../../common/Preloaders/Preloader";
import {getEvent, toggleIsFetching, updateEvent} from "../../../../redux/eventManagerArea-reducer";
import UpdateEventForm from "./UpdateEventForm";
import {compose} from "redux";
import {withRouter} from "react-router";

class UpdateEventContainer extends React.Component {
    componentDidMount() {
        this.props.toggleIsFetching(true);
        let eventId = this.props.match.params.eventId;
        if(!this.props.event) {
            this.props.getEvent(eventId);
        }
    }
    render() {
        return <>
            {this.props.isFetching ? <Preloader/>
                : <UpdateEventForm {...this.props}
                                   onSubmit={(formData) => {
                                       this.props.updateEvent(formData, this.props.match.params.eventId)}}/>
            }
        </>
    }
}

let mapStateToProps = (state) => {
    return {
        isAuth: state.authPage.isAuth,
        event: state.eventManagerAreaPage.event,
        events: state.eventManagerAreaPage.events,
        isFetching: state.eventManagerAreaPage.isFetching,
        isUpdateEventSuccessful: state.eventManagerAreaPage.isUpdateEventSuccessful,
    }
}

export default compose(
    connect(
    mapStateToProps,
    {
        getEvent, updateEvent, toggleIsFetching}),
    withRouter,
)(UpdateEventContainer);