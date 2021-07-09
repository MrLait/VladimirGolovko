import React from "react";
import {useTranslation} from "react-i18next";
import "./Basket.module.css"
import basePath from "../../assets/Pics/FootballPicOne.PNG";
import {Link} from "react-router-dom";

let Basket = (props) => {
    const {t} = useTranslation();
    const buyOnClick = () => {
        props.buy(props.totalPrice, props.balance, props.items);
    }
    return (
        <div>
            {props.items && props.items.length  ?
                <table>
                    <tr>
                        <th>{t('Picture')}</th>
                        <th>{t('EventName')}</th>
                        <th>{t('AreaDescription')}</th>
                        <th>{t('RowNumber')}</th>
                        <th>{t('NumberOfSeat')}</th>
                        <th>{t('EventStartsAt')}</th>
                        <th>{t('EventEndsAt')}</th>
                        <th>{t('Price')}</th>
                    </tr>
                    {props.items.map(
                        e =>
                            <tr key={e.id}>
                                <td>
                                    <div><img src={basePath}/></div>
                                </td>
                                <td>{e.eventName}</td>
                                <td>{e.eventAreaDescription}</td>
                                <td>{e.row}</td>
                                <td>{e.numberOfSeat}</td>
                                <td>{e.eventDateTimeStart}</td>
                                <td>{e.eventDateTimeEnd}</td>
                                <td>{e.price}</td>
                            </tr>
                    )}
                    <div> {(t)('TotalPrice')}: {props.totalPrice}</div>
                    {props.isAuth ?
                        <button onClick={buyOnClick}>{t('Buy')} </button>
                        :
                        <Link to={"/login"}>
                            <button>{t('Buy')} </button>
                        </Link>
                    }
                    <div>{props.isNotEnoughMoney ? "NotEnoughMoney" : ""} </div>
                </table>
                : t('PurchaseHistoryIsEmpty')}
        </div>
    )
}
export default Basket;