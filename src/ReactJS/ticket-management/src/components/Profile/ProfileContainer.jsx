import React from "react";
import Preloader from "../../common/Preloaders/Preloader";
import {deposit, getBalance, toggleIsFetching} from "../../redux/profile-reducer";
import Profile from "./Profile";
import {connect} from "react-redux";

class ProfileContainer extends React.Component {
    componentDidMount() {
        this.props.getBalance()
    }
    render() {
        return <>
            {this.props.isFetching ? <Preloader /> : <Profile {...this.props}/>}
        </>
    }
}

let mapStateToProps = (state) => {
    return {
        clickingInProgress: state.profilePage.clickingInProgress,
        isFetching: state.profilePage.isFetching,
        balance: state.profilePage.balance,
        isAuth: state.authPage.isAuth
    }
}

export default connect(
    mapStateToProps,
    {
        toggleIsFetching,
        getBalance,
        deposit
        }) (ProfileContainer);