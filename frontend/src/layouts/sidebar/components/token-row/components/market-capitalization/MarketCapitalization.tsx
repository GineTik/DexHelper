import { useEffect, useState } from "react"

type MarketCapitalizationProps = {
    capitalization: number
}

const MarketCapitalization = ({capitalization}: MarketCapitalizationProps) => {
    const [mcap, setMCap] = useState<string>((isNaN(capitalization) ? 0 : capitalization).toString())
    useEffect(() => {
        setMCap(ParseLifeTimeSecondsToString(capitalization).toString())
    }, [capitalization])

    return (
        <span>
            ${mcap}
        </span>
    )
}

const ParseLifeTimeSecondsToString = (capitalization: number) => {
    if (isNaN(capitalization))
        return "0"
    if (capitalization > 1000)
        return (capitalization / 1000).toFixed(2) + "k"

    return capitalization ?? "???"
}

export default MarketCapitalization
