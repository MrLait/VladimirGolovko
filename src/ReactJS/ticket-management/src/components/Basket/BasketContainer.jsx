import React from "react";
import {connect} from "react-redux";
import {toggleIsFetching} from "../../redux/events-reducer";
import Preloader from "../../common/Preloaders/Preloader";
import {buy, getBalance, getUserItems} from "../../redux/basket-reducer";
import Basket from "./Basket";
import {ifNotAuthRedirectToHome} from "../../hoc/ifNotAuthRedirectToHome";
import {compose} from "redux";

class BasketContainer extends React.Component {
    componentDidMount() {
        this.props.getUserItems();
        this.props.getBalance();
    }

    render() {
        return <>
            {this.props.isFetching ? <Preloader /> : <Basket {...this.props}/>}
        </>
    }
}

let mapStateToProps = (state) => {
    return {
        items: state.basketPage.items,
        isFetching: state.basketPage.isFetching,
        totalPrice: state.basketPage.totalPrice,
        balance: state.basketPage.balance,
        isNotEnoughMoney: state.basketPage.isNotEnoughMoney,
        inProgress: state.basketPage.inProgress,
        isBuySuccessful: state.basketPage.isBuySuccessful
    }
}

export default compose(
    connect(
        mapStateToProps,
        {
            toggleIsFetching, getUserItems, buy, getBalance
        }),
    ifNotAuthRedirectToHome
)(BasketContainer);