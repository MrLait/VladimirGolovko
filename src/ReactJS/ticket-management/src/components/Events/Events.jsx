import React from "react";
import {Link} from "react-router-dom";
import {useTranslation} from "react-i18next";
import style from "./Events.module.css"

let Events = (props) => {
    const {t} = useTranslation();
    debugger;
    return (
        <table>
            <tr>
                <th>{t('Picture')}</th>
                <th>{t('EventName')}</th>
                <th>{t('Description')}</th>
                <th>{t('EventStartsAt')}</th>
                <th>{t('EventEndsAt')}</th>
                <th>{t('AvailableOfSeats')}</th>
                <th>{t('PricesStartFrom')}</th>
                <th>{t('PricesTo')}</th>
                <th>{t('ChooseTickets')}</th>
            </tr>
            {props.events.map(
                e =>
                    <tr >
                        <td>
                            <div><img className={style.img} src= {`${window.location.origin}/${e.imageUrl}`}/></div>
                        </td>
                        <td>{e.name}</td>
                        <td>{e.description}</td>
                        <td>{e.startDateTime}</td>
                        <td>{e.endDateTime}</td>
                        <td>{e.availableSeats}</td>
                        <td>{e.priceFrom}</td>
                        <td>{e.priceTo}</td>
                        <td>
                            {props.isAuth ?
                                <Link to={"/eventArea/" + e.id}>
                                    <button>{t('ChooseTickets')} </button>
                                </Link>
                                :
                                <Link to={"/login"}>
                                    <button>{t('ChooseTickets')} </button>
                                </Link>
                            }
                        </td>
                    </tr>
            )}
        </table>
    )
}
export default Events;