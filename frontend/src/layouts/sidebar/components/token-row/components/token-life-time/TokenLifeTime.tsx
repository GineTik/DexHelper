import { useEffect, useState } from "react"

type TokenLifeTimeProps = {
    dateUtc: string
}

const TokenLifeTime = ({dateUtc}: TokenLifeTimeProps) => {
    const [lifeTime, setLifeTime] = useState("0s")

    useEffect(() => {
        const date = new Date(dateUtc + 'Z');
        var now = new Date();
        var timeDiff = now.valueOf() - date.valueOf();
        var seconds = Math.round(timeDiff / 1000);
        setLifeTime(ParseLifeTimeSecondsToString(seconds))

        const timeoutId = setTimeout(() => {
            seconds += 1;
            setLifeTime(ParseLifeTimeSecondsToString(seconds))
        }, 1000);
      
        return () => clearTimeout(timeoutId);
    })

    return (
        <span>
            {lifeTime}
        </span>
    )
}

const ParseLifeTimeSecondsToString = (lifeTimeSeconds: number) => {
    var seconds = Math.floor(lifeTimeSeconds % 60)
    var minutes = Math.floor(lifeTimeSeconds / 60)
    var hours = Math.floor(minutes / 60)
    var days = Math.floor(hours / 24)

    if (days > 0)
        return days + "d"
    if (hours > 0)
        return hours + "h"
    if (minutes > 0)
        return minutes + "m"
    else 
        return seconds + "s"
}

export default TokenLifeTime
