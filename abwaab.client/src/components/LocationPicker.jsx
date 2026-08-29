import React, { useEffect, useState } from "react";
import { MapContainer, TileLayer, Marker, useMapEvents } from "react-leaflet";
import L from "leaflet";

// Fix Leaflet's default marker icon path issue in Webpack/Vite
import markerIcon2x from "leaflet/dist/images/marker-icon-2x.png";
import markerIcon from "leaflet/dist/images/marker-icon.png";
import markerShadow from "leaflet/dist/images/marker-shadow.png";

delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconUrl: markerIcon,
  iconRetinaUrl: markerIcon2x,
  shadowUrl: markerShadow,
});

// Listener component to capture click events on the map
function LocationMarker({ position, setPosition, readOnly = false }) {
  useMapEvents({
    click(e) {
      setPosition(e.latlng); // Updates state with { lat, lng }
    },
  });

  return position === null ? null : (
    <Marker
      position={position}
      draggable={!readOnly}
      eventHandlers={{
        dragend: (e) => {
          setPosition(e.target.getLatLng());
        },
      }}
    />
  );
}

export const LocationPicker = ({
  lat = 33.5138,
  lng = 36.2765,
  onLocationSelect,
  readOnly = false,
}) => {
  // Default coordinates (e.g., Damascus: 33.5138, 36.2765)
  const defaultCenter = [lat, lng];
  const [position, setPosition] = useState(null);

  const handleSetPosition = (latlng) => {
    if (!readOnly) {
      setPosition(latlng);
      if (onLocationSelect) {
        onLocationSelect({
          lat: latlng.lat,
          lng: latlng.lng,
        });
      }
    }
  };
  useEffect(() => {
    setPosition({ lat, lng });
  }, [lng, lat]);
  return (
    <div className="w-full space-y-2">
      <div className="h-64 w-full rounded-lg overflow-hidden border border-teal-400 shadow-inner">
        <MapContainer
          center={defaultCenter}
          zoom={13}
          scrollWheelZoom={true}
          style={{ height: "100%", width: "100%" }}
        >
          {/* Free OpenStreetMap Tiles */}
          <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          />
          <LocationMarker
            position={position}
            setPosition={handleSetPosition}
            readOnly={readOnly}
          />
        </MapContainer>
      </div>

      {position && !readOnly && (
        <p className="text-xs text-slate-600 font-mono text-center">
          Selected: {position.lat.toFixed(6)}, {position.lng.toFixed(6)}
        </p>
      )}
    </div>
  );
};
