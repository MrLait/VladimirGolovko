import React from "react";
import {connect} from "react-redux";
import Preloader from "../../../../common/Preloaders/Preloader";
import {
    getEvent,
    updateEvent
} from "../../../../redux/eventManagerArea-reducer";
import UpdateEventForm from "./UpdateEventForm";
import {compose} from "redux";
import {withRouter} from "react-router";

class UpdateEventContainer extends React.Component {
    componentDidMount() {
        let eventId = this.props.match.params.eventId;
        this.props.getEvent(eventId);
        debugger;
    }

    render() {
        return <>
            {this.props.isFetching ? <Preloader/>
                : <UpdateEventForm {...this.props}
                                   onSubmit={(formData) => {
                                       this.props.updateEvent(formData)}}/>
            }
        </>
    }
}

let mapStateToProps = (state) => {
    return {
        isAuth: state.authPage.isAuth,
        event: state.eventManagerAreaPage.event,
        isFetching: state.eventManagerAreaPage.isFetching,
        isUpdateEventSuccessful: state.eventManagerAreaPage.isUpdateEventSuccessful,
        initialValues: state.eventManagerAreaPage.event,
    }
}

export default compose(
    connect(
    mapStateToProps,
    {
        getEvent, updateEvent}),
    withRouter,
)(UpdateEventContainer);