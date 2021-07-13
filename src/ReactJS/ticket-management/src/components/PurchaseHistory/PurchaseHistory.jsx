import React from "react";
import {useTranslation} from "react-i18next";
import style from "./PurchaseHistory.module.css"

let PurchaseHistory = (props) => {
    const {t} = useTranslation();
    debugger;
    return (
        <div>
            {props.items && props.items.length ?
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
                </table>
                : t('PurchaseHistoryIsEmpty')
            }
        </div>
    )
}
export default PurchaseHistory;