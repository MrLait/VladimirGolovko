import React from "react";
import {useTranslation} from "react-i18next";
import basePath from "../../../assets/Pics/FootballPicOne.PNG";
import {Link} from "react-router-dom";

const EventManagerArea = (props) => {
    const {t} = useTranslation();
    return (
        <div>
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
                </tr>
                {props.events.map(
                    e =>
                        <tr>
                            <td>
                                <div><img src={basePath}/></div>
                            </td>
                            <td>{e.name}</td>
                            <td>{e.description}</td>
                            <td>{e.startDateTime}</td>
                            <td>{e.endDateTime}</td>
                            <td>{e.availableSeats}</td>
                            <td>{e.priceFrom}</td>
                            <td>{e.priceTo}</td>
                            <td>
                                <button onClick={"delete"}>{t('Delete')} </button>
                                <Link to={"update/event/" + e.id}>
                                    <button>{t('Update')} </button>
                                </Link>
                            </td>
                        </tr>
                )}
            </table>
            <div>
                <Link to={"create/event"}>
                    <button>{t('Create event')} </button>
                </Link>
            </div>
        </div>
    )
}

export default EventManagerArea;
