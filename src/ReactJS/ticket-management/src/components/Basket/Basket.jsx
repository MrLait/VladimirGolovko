import React from "react";
import {useTranslation} from "react-i18next";
import "./Basket.module.css"
import {Link} from "react-router-dom";
import style from "./Basket.module.css"

let Basket = (props) => {
    const {t} = useTranslation();
    const buyOnClick = () => {
        props.buy(props.totalPrice, props.balance, props.items);
    }
    debugger;
    return (
        <div>
            {props.items && props.items.length
                ? <table>
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
                    {props.items.map(e =>
                        <tr>
                            <td>
                                <div><img className={style.img} src={`${window.location.origin}/${e.pictureUrl}`}/></div>
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
                    <div>{`${t("Total price")}: ${props.totalPrice}`}</div>
                    {props.isAuth ?
                        <button disabled={props.inProgress} onClick={buyOnClick}>{t('Buy')} </button>
                        :
                        <Link to={"/login"}>
                            <button>{t('Buy')}</button>
                        </Link>
                    }
                    <div>{props.isNotEnoughMoney ? t('NotEnoughMoney') : ""} </div>
                </table>
                : t('Basket is empty')}
            <div>
                {props.isBuySuccessful ? t('The purchase was successful') : "" }
            </div>
        </div>
    )
}
export default Basket;