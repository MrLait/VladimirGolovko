import React from "react";
import {connect} from "react-redux";
import {toggleIsFetching} from "../../redux/events-reducer";
import Preloader from "../../common/Preloaders/Preloader";
import PurchaseHistory from "./PurchaseHistory";
import {getUserItems} from "../../redux/purchaseHisory-reducer";

class PurchaseHistoryContainer extends React.Component {
    componentDidMount() {
        this.props.toggleIsFetching(true);
        this.props.getUserItems();
    }
    render() {
        return <>
            {this.props.isFetching ? <Preloader /> : null}
            <PurchaseHistory {...this.props}/>
        </>
    }
}

let mapStateToProps = (state) => {
    return {
        isAuth: state.authPage.isAuth,
        items: state.purchaseHistoryPage.items,
        isFetching: state.basketPage.isFetching,
    }
}

export default connect(
    mapStateToProps,
    {
        toggleIsFetching, getUserItems
        }) (PurchaseHistoryContainer);