# Final Discounted Price - Database Storage Implementation

## ? Completed Changes

### What Was Added

I've successfully implemented storing the final calculated discount amount in the database. Here's what changed:

### 1. **HotelRoom Entity** (Domain Layer)
Added a new property to store the calculated discounted price:
```csharp
public decimal FinalDiscountedPrice { get; set; }
```

This property is **stored in the database** and is automatically calculated when you create or update a hotel.

### 2. **Database Migration** (Infrastructure Layer)
Created migration: `20251211152511_AddDiscountFieldsToHotelRoom`

Adds two columns to the `HotelRooms` table:
- `DiscountPercentage` (decimal 18,2) - The discount percentage to apply
- `FinalDiscountedPrice` (decimal 18,2) - **The calculated final price after discount**

### 3. **Automatic Calculation** (Application Layer)
When creating or updating hotels, the service automatically:
1. Takes the `PricePerNight`
2. Takes the `DiscountPercentage`
3. Calculates: `FinalDiscountedPrice = PricePerNight - (PricePerNight * DiscountPercentage / 100)`
4. **Saves this value to the database**

### 4. **DTO Updates**
The `CreateHotelRoomDto` now includes:
```csharp
public decimal DiscountPercentage { get; set; } = 0;
public decimal FinalDiscountedPrice { get; set; }
```

## How It Works

### When Creating a Hotel:
```json
POST /api/Hotel
{
  "hotelName": "My Hotel",
  "rooms": [
    {
      "roomType": 0,
      "pricePerNight": 100.00,
      "discountPercentage": 20.0,
      "totalRooms": 10,
      "maxOccupancy": 2,
      "bedCount": 1,
      "roomSize": 25.0,
      "description": "Standard room"
    }
  ]
}
```

**What happens:**
1. System receives: PricePerNight = $100, DiscountPercentage = 20%
2. System calculates: FinalDiscountedPrice = $100 - ($100 × 20 / 100) = **$80.00**
3. System saves to database:
   - `PricePerNight`: 100.00
   - `DiscountPercentage`: 20.0
   - `FinalDiscountedPrice`: **80.00** ? Stored in database!

### When Retrieving a Hotel:
```json
GET /api/Hotel/1

Response:
{
  "hotelId": 1,
  "rooms": [
    {
      "pricePerNight": 100.00,
      "discountPercentage": 20.0,
      "finalDiscountedPrice": 80.00  ? Retrieved from database!
    }
  ]
}
```

## Apply the Migration

### Option 1: .NET CLI
```bash
cd Infrastructure_Layer
dotnet ef database update --startup-project ..\Presentation_Layer\Presentation_Layer.csproj
```

### Option 2: Visual Studio Package Manager Console
```powershell
Update-Database
```

## Database Schema After Migration

**HotelRooms Table:**
| Column Name | Type | Description |
|-------------|------|-------------|
| HotelRoomId | int | Primary Key |
| HotelId | int | Foreign Key |
| RoomType | int | Enum value |
| PricePerNight | decimal(18,2) | Original price |
| **DiscountPercentage** | decimal(18,2) | **Discount % (e.g., 15.0 = 15%)** |
| **FinalDiscountedPrice** | decimal(18,2) | **Calculated & stored final price** |
| MaxOccupancy | int | Max guests |
| BedCount | int | Number of beds |
| RoomSize | decimal(18,2) | Room size in sqm |
| Description | nvarchar(max) | Room description |
| TotalRooms | int | Available rooms |

## Benefits

? **Persistent Storage**: The discounted price is saved in the database  
? **No Recalculation Needed**: Just read the value from the database  
? **Historical Data**: Preserves the price at the time of creation  
? **Performance**: Faster queries - no need to calculate on every read  
? **Reporting**: Easy to generate reports with final prices  
? **Automatic**: You don't need to calculate manually - the system does it  

## Examples

### Example 1: 15% Discount
- Original: $200/night
- Discount: 15%
- **Stored in DB**: $170.00

### Example 2: 25% Discount
- Original: $150/night
- Discount: 25%
- **Stored in DB**: $112.50

### Example 3: No Discount
- Original: $100/night
- Discount: 0%
- **Stored in DB**: $100.00

## Build Status
? **Build Successful** - All changes compile without errors

## Next Steps
1. ? Run the migration (see commands above)
2. ? Create or update hotels with discount percentages
3. ? The `FinalDiscountedPrice` will be automatically calculated and stored in the database
4. ? Retrieve hotel data and see the stored discounted prices

---

**Note:** The system still has the calculated `DiscountedPrice` property for on-the-fly calculations, but now you also have `FinalDiscountedPrice` persisted in the database for faster access and historical tracking!
